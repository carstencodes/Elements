#!/usr/bin/env bash

if [ -n "${CODESPACE_NAME}" ]; then
    echo -e 'The codespace user does not require file ownership mappings. Skipping ...'
    exit 0
fi

if [ "$(id -u)" -ne 0 ]; then
    echo -e 'Script must be run as root. Use sudo, su, or add "USER root" to your Dockerfile before running this script.'
    exit 1
fi

USER_NAME=${USERNAME:-"developer"}
UID_TO_SET=${UID:-1000}
GID_TO_SET=${GID:-1000}

id ${UID_TO_SET} &>/dev/null && usermod -l ${USER_NAME} $(id -nu ${UID_TO_SET}) || adduser --uid ${UID_TO_SET} --gid ${GID_TO_SET} --disabled-password --gecos "" ${USER_NAME}

id -G ${USER_NAME} | grep -q ${GID_TO_SET} || usermod -aG $(getent group ${GID_TO_SET} | cut -d: -f1) ${USER_NAME}
