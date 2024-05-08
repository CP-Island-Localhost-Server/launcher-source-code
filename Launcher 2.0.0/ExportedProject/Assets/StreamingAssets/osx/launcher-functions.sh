#!/bin/bash
# test-functions.sh


#-------------------------------------------------------------------------------
# argument 1: Process ID to bring to the front. 
function bring_process_to_front ()
{
    osascript<<END_OF_OSA
    tell application "System Events"
        set processList to every process whose unix id is ${1}
        repeat with proc in processList
            set the frontmost of proc to true
        end repeat
    end tell
END_OF_OSA
}


#-------------------------------------------------------------------------------
# This is what the OSX 'open' command is suppose to do, however,
# when 'open -a Client.app' is executed from Unity via
# the C# 'Process' class the Client app does not execute.
#-------------------------------------------------------------------------------
# $1: Path name to App. 
# $2,...: command line arguments to $1
function run_app_in_front ()
{
    if [ $# == 0 ]
    then
        echo "ERROR: run_app_in_front () -- NO ARGS."
        return 1
    fi

    local RUN_APP_PATH="$1"
    echo "Launching App $RUN_APP_PATH"
    shift 1

    local APP_INFO_PLIST=$(defaults read "$RUN_APP_PATH/Contents/Info.plist")
    local REGEX_EXECUTABLE='CFBundleExecutable[[:space:]]*=[[:space:]]*"([^"]*)"'
    if [[ "$APP_INFO_PLIST" =~ $REGEX_EXECUTABLE ]]
    then
        # Directly run the executable.
        # This has been shown to work more consistently than using the 'open' command.
        echo "$RUN_APP_PATH/Contents/MacOS/${BASH_REMATCH[1]} $@"
        "$RUN_APP_PATH/Contents/MacOS/${BASH_REMATCH[1]}" "$@" &
        local APP_PID=$!
    else
        # Fallback to using the 'open' command, however, this has been shown to not work in all situations.
        echo "open -a $RUN_APP_PATH --args $@"
        open -a "$RUN_APP_PATH" --args "$@" &
        local APP_PID=$!
    fi

    echo "Bring PID $APP_PID to front."
    bring_process_to_front $APP_PID
}
