#!/bin/bash

# wait for database to start...
for i in {30..0}; do
  if /opt/mssql-tools/bin/sqlcmd -U SA -P ${SA_PASSWORD} -Q 'SELECT 1;' &> /dev/null; then
    echo "$0: SQL Server started"
    break
  fi
  echo "$0: SQL Server startup in progress..."
  sleep 1
done

echo "$0: Initializing database by parameter 2"
/opt/mssql-tools/bin/sqlcmd \
   -U SA -P ${SA_PASSWORD} \
   -Q 'CREATE DATABASE [Weather]'
echo "$0: SQL Server Database ready"
