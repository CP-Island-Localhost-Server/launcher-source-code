#!/bin/bash
# run-client-in-front.sh
#
# https://www.gnu.org/software/bash/manual/html_node/The-Set-Builtin.html
set -o errexit
set -o nounset

# $SCRIPT is the filename of this script without the path.
declare -rx SCRIPT=${0##*/}
# $SCRIPT_DIR is the path to this script.
SCRIPT_DIR="$(dirname "$0")"

source "$SCRIPT_DIR/launcher-functions.sh"

run_app_in_front "$@"
