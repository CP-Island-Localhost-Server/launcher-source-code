# Bash Script to create sym links in the Launcher App from the Game Client App
#
# Required Environment Variables:
#  CPI_LAUNCHER_INSTALL_DIRECTORY - Launcher Install Directory
#  CPI_CLIENT_INSTALL_DIRECTORY   - Game Client Install Directory
#
# https://www.gnu.org/software/bash/manual/html_node/The-Set-Builtin.html
set -o errexit
set -o nounset

# $SCRIPT is the filename of this script without the path.
declare -rx SCRIPT=${0##*/}
echo "SCRIPT=$SCRIPT"
# $SCRIPT_DIR is the path to this script.
SCRIPT_DIR="$(dirname "$0")"
echo "SCRIPT_DIR=$SCRIPT_DIR"

# Echo the required environment variables.
# If any are not set then this script will terminate before executing any further commands.
echo "CPI_LAUNCHER_INSTALL_DIRECTORY=$CPI_LAUNCHER_INSTALL_DIRECTORY"
echo "CPI_CLIENT_INSTALL_DIRECTORY=$CPI_CLIENT_INSTALL_DIRECTORY"

DST_ZF_GAME_BROWSER_PATH="ZFGameBrowser.app"
SRC_ZF_GAME_BROWSER_PATH="$CPI_CLIENT_INSTALL_DIRECTORY/Contents/Frameworks/$DST_ZF_GAME_BROWSER_PATH"

DST_CEF_PATH="Chromium Embedded Framework.framework"
SRC_CEF_PATH="$CPI_CLIENT_INSTALL_DIRECTORY/Contents/Frameworks/$DST_CEF_PATH"


pushd "$CPI_LAUNCHER_INSTALL_DIRECTORY/Contents/Frameworks"

# Always remove existing links in case the locations change between updates.
if [ -h "$DST_ZF_GAME_BROWSER_PATH" ]
then
    rm "$DST_ZF_GAME_BROWSER_PATH"
fi
ln -s "$SRC_ZF_GAME_BROWSER_PATH" "$DST_ZF_GAME_BROWSER_PATH"

if [ -h "$DST_CEF_PATH" ]
then
    rm "$DST_CEF_PATH"
fi
ln -s "$SRC_CEF_PATH" "$DST_CEF_PATH"

popd
exit 0
