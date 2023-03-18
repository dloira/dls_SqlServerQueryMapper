FROM mcr.microsoft.com/mssql/server:2022-latest
USER root
EXPOSE 1433
COPY sqlserver/docker-entrypoint.sh /usr/local/bin/
COPY sqlserver/docker-entrypoint-initdb.sh /usr/local/bin/
RUN chmod 777 /usr/local/bin/docker-entrypoint.sh \
    && ln -s /usr/local/bin/docker-entrypoint.sh /
RUN chmod 777 /usr/local/bin/docker-entrypoint-initdb.sh \
    && ln -s /usr/local/bin/docker-entrypoint-initdb.sh /
RUN apt-get update
RUN apt-get install dos2unix
RUN dos2unix -F /docker-entrypoint*
ENTRYPOINT ["/docker-entrypoint.sh"]