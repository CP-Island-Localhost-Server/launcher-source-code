# Script to wait for the Launcher to exit then update the Launcher.
#
# Required Environment Variables:
#  CPI_LAUNCHER_PID.              - the self updater script must first wait for this process to quit.
#  CPI_LAUNCHER_INSTALL_DIRECTORY - Launcher Install Directory
#  CPI_LAUNCHER_DMG_FILENAME      - Downloaded Launcher DMG Filename
#  CPI_PACKAGED_LAUNCHER_APP_NAME - Packaged Launcher App Name
#
# https://www.gnu.org/software/bash/manual/html_node/The-Set-Builtin.html
set -o errexit
set -o nounset

# $SCRIPT is the filename of this script without the path.
declare -rx SCRIPT=${0##*/}
# $SCRIPT_DIR is the path to this script.
SCRIPT_DIR="$(dirname "$0")"

source "$SCRIPT_DIR/launcher-functions.sh"


# The maximum number of seconds to wait for the Launcher to exit.
# Adjust this value as experience dictates.
MAXIMUM_WAIT_SECONDS=60

CPI_LAUNCHER_DMG_MOUNT_POINT="/Volumes/ClubPenguinIslandLauncher"

# Echo the required environment variables.
# If any are not set then this script will terminate before executing any further commands.
echo "CPI_LAUNCHER_PID=$CPI_LAUNCHER_PID"
echo "CPI_LAUNCHER_INSTALL_DIRECTORY=$CPI_LAUNCHER_INSTALL_DIRECTORY"
echo "CPI_LAUNCHER_DMG_FILENAME=$CPI_LAUNCHER_DMG_FILENAME"
echo "CPI_PACKAGED_LAUNCHER_APP_NAME=$CPI_PACKAGED_LAUNCHER_APP_NAME"

if [ $CPI_LAUNCHER_PID == "0" ]
then
    #cannot self-update in dev mode from the editor.
    exit 0
fi


#------------------------------------------
# Attach/Mount the DMG file before wait for the launcher to exit.
# This is a bit more efficient use of time.
echo "mounting $CPI_LAUNCHER_DMG_FILENAME"
if [ -d "$CPI_LAUNCHER_DMG_MOUNT_POINT" ]
then
  # Something is still mounted at that mount point. Unmount it.
  hdiutil detach "$CPI_LAUNCHER_DMG_MOUNT_POINT" -force
fi
hdiutil attach "$CPI_LAUNCHER_DMG_FILENAME" -mountpoint "$CPI_LAUNCHER_DMG_MOUNT_POINT" -nobrowse
# If something goes wrong, make sure the dmg file gets detached.
trap 'hdiutil detach "$CPI_LAUNCHER_DMG_MOUNT_POINT" -force' EXIT


#------------------------------------------
echo "Waiting for Launcher to exit. (PID=$CPI_LAUNCHER_PID)"
while ps -p $CPI_LAUNCHER_PID > /dev/null
do
    sleep 1
    # Test for time overrun.
    if [ $SECONDS -gt $MAXIMUM_WAIT_SECONDS ]
    then
        break
    fi
done

ELAPSED_SECONDS=$SECONDS
#echo "ELAPSED_SECONDS=$ELAPSED_SECONDS"
if [ $ELAPSED_SECONDS -gt $MAXIMUM_WAIT_SECONDS ]
then
    echo "Waited $ELAPSED_SECONDS seconds for Launcher to exit."
    echo "Maximum wait is $MAXIMUM_WAIT_SECONDS seconds."
    echo "Exiting without further actions."
    exit 2
fi


#------------------------------------------
echo "Updating Launcher."
# The following updates this script file as well.
# from: https://unix.stackexchange.com/questions/138214/how-is-it-possible-to-do-a-live-update-while-a-program-is-running
# ...the old script file remains open during the upgrade.
# ...the step of removing the file in fact only removes the file's entry in the directory.
# The file itself is only removed when it has no directory entry leading to it and no process has it open.
# Then 'cp' creates a new directory entry leading to the new file.

# Delete this script file now so that it can be updated in the next step without interrupting execution.
# Deleting it here also prevents an infinite self-update loop if version number are not configured correctly.
# Also, do not delete the entire Launcher App folder because the Client exists inside this folder and
#  it may not need updating.
rm -f "$0"

# Update the Launcher App.
if [ -d "$CPI_LAUNCHER_INSTALL_DIRECTORY" ]
then
    # The target directory already exists.
    # 'cd' in order to more easily facilitate wild card usage below.
    cd "$CPI_LAUNCHER_INSTALL_DIRECTORY"

    # Delete old files from the Content/MacOS directory to avoid App Signing issues.
    rm -Rf Contents/MacOS/*
    # And clean up the contents of these folders in case we are updating the version of Unity.
    rm -Rf Contents/Mono/*
    rm -Rf Contents/MonoBleedingEdge/*
    rm -Rf Contents/Resources/*

    # Copy the new files into the target directory.
    cp -Rf "$CPI_LAUNCHER_DMG_MOUNT_POINT/$CPI_PACKAGED_LAUNCHER_APP_NAME/" "$CPI_LAUNCHER_INSTALL_DIRECTORY"
else
    # The target directory does not yet exist, so create it as part of the copying.
    # Under normal user flow this should not happen but this is here for completeness and to cover unexpected edge cases.
    cp -Rf "$CPI_LAUNCHER_DMG_MOUNT_POINT/$CPI_PACKAGED_LAUNCHER_APP_NAME" "$CPI_LAUNCHER_INSTALL_DIRECTORY"
fi

echo "unmounting $CPI_LAUNCHER_DMG_MOUNT_POINT"
hdiutil detach "$CPI_LAUNCHER_DMG_MOUNT_POINT" -force
trap EXIT #reset the 'trap' from further above.

# The DMG file is no longer needed.
rm -f "$CPI_LAUNCHER_DMG_FILENAME"


#------------------------------------------
# Run the newly updated Launcher.
echo "re-launch $CPI_LAUNCHER_INSTALL_DIRECTORY"
run_app_in_front "$CPI_LAUNCHER_INSTALL_DIRECTORY"

exit 0
