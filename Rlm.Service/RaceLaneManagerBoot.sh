#! /bin/sh
# /etc/init.d/RaceLaneManagerBoot.sh

### BEGIN INIT INFO
# Provides: RaceLaneManagerBoot
# Required-Start: $remote_fs $syslog
# Required-Stop: $remote_fs $syslog
# Default-Start: 2 3 4 5
# Default-Stop: 0 1 6
# Short-Description: Start RaceLaneManager at boot time
# Description: Start RaceLaneManager at boot time.
### END INIT INFO

USER=pi
HOME=/home/pi

export USER HOME

case "$1" in
 start)
  echo "Starting RaceLaneManager"
  sudo $HOME/RaceLaneManager/RlmDemon.sh start
  ;;

 stop)
  echo "Stopping RaceLaneManager"
  sudo $HOME/RaceLaneManager/RlmDemon.sh stop
  ;;

 *)
  echo "Usage: /etc/init.d/RaceLaneManager.sh {start|stop}"
  exit 1
  ;;
esac

exit 0
