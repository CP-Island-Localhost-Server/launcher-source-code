# Bash Script to install the Game Client.
#
# Required Environment Variables:
#  CPI_LAUNCHER_INSTALL_DIRECTORY - Launcher Install Directory
#  CPI_CLIENT_INSTALL_DIRECTORY   - Game Client Install Directory
#  CPI_CLIENT_DMG_FILENAME        - Downloaded Client DMG Filename
#  CPI_PACKAGED_CLIENT_APP_NAME   - Packaged Game Client App Name
#  CPI_CREATE_SYM_LINKS           - Create Mac Sym Links - "true", "false"
#
# https://www.gnu.org/software/bash/manual/html_node/The-Set-Builtin.html
set -o errexit
set -o nounset

# $SCRIPT is the filename of this script without the path.
declare -rx SCRIPT=${0##*/}
# $SCRIPT_DIR is the path to this script.
SCRIPT_DIR="$(dirname "$0")"

source "$SCRIPT_DIR/launcher-functions.sh"


CPI_CLIENT_DMG_MOUNT_POINT="/Volumes/ClubPenguinIsland"

# Echo the required environment variables.
# If any are not set then this script will terminate before executing any further commands.
echo "CPI_LAUNCHER_INSTALL_DIRECTORY=$CPI_LAUNCHER_INSTALL_DIRECTORY"
echo "CPI_CLIENT_INSTALL_DIRECTORY=$CPI_CLIENT_INSTALL_DIRECTORY"
echo "CPI_CLIENT_DMG_FILENAME=$CPI_CLIENT_DMG_FILENAME"
echo "CPI_PACKAGED_CLIENT_APP_NAME=$CPI_PACKAGED_CLIENT_APP_NAME"
echo "CPI_CREATE_SYM_LINKS=$CPI_CREATE_SYM_LINKS"


#----------------------------------------
echo "mounting $CPI_CLIENT_DMG_FILENAME"
if [ -d "$CPI_CLIENT_DMG_MOUNT_POINT" ]
then
  # Something is still mounted at that mount point. Unmount it.
  hdiutil detach "$CPI_CLIENT_DMG_MOUNT_POINT" -force
fi
hdiutil attach "$CPI_CLIENT_DMG_FILENAME" -mountpoint "$CPI_CLIENT_DMG_MOUNT_POINT" -nobrowse
# If something goes wrong, make sure the dmg file gets detached.
trap 'hdiutil detach "$CPI_CLIENT_DMG_MOUNT_POINT" -force' EXIT


#-------------------------
echo "Updating Game Client."
# Remove the old app if it exists so there aren't any lingering files.
if [ -d "$CPI_CLIENT_INSTALL_DIRECTORY" ]
then
    rm -Rf "$CPI_CLIENT_INSTALL_DIRECTORY"
fi

cp -Rf "$CPI_CLIENT_DMG_MOUNT_POINT/$CPI_PACKAGED_CLIENT_APP_NAME" "$CPI_CLIENT_INSTALL_DIRECTORY"

echo "unmounting $CPI_CLIENT_DMG_MOUNT_POINT"
hdiutil detach "$CPI_CLIENT_DMG_MOUNT_POINT"
trap EXIT #reset the 'trap' from further above.

# The DMG file is no longer needed.
rm -f "$CPI_CLIENT_DMG_FILENAME"


#-------------------------
# Create Mac Sym Links
if [ "$CPI_CREATE_SYM_LINKS" == "true" ]
then
    echo "Creating Mac Sym Links."
    bash "$SCRIPT_DIR/create-sym-links.sh"
fi

exit 0
