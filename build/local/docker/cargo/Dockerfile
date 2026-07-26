ARG BASE_IMAGE=joseluisq/rust-linux-darwin-builder:2.0.0-beta.1

FROM ${BASE_IMAGE}

USER 0

ARG TOOLCHAIN=1.94.0
ENV TOOLCHAIN=${TOOLCHAIN}

RUN --mount=type=bind,source=./deps,target=/tmp/build-context \
    apt update && \
        apt install -y --no-install-recommends gosu && \
        apt-get clean && \
        rm -rf /var/lib/apt/lists/* && \
    rustup target list --installed > /tmp/toolchains && \
        rustup toolchain install ${TOOLCHAIN} && \
        rustup default ${TOOLCHAIN} && \
        rustup target add --toolchain ${TOOLCHAIN} $(cat /tmp/toolchains) && \
    rm -f /tmp/toolchains && \
    cat /tmp/build-context/cargo-tools.ini | \
        tr -d ' \t' | \
        grep -vE '^\[' | \
        grep -vE '^;' | \
        tr '=' '@' | \
        xargs cargo install --locked \
    && touch /usr/local/bin/cbuild \
    && chmod a+x /usr/local/bin/cbuild \
    && cat << 'EOF' > /usr/local/bin/cbuild
#!/bin/sh

set -eux

export GOSU_SPEC=
if [ -n "${DUID:-}" ]
then
    export GOSU_SPEC=${DUID}
fi

if [ -n "${DGID:-}" ]
then
    export GOSU_SPEC=${GOSU_SPEC}:${DGID}
fi

export SCALL=
if [ -n "${GOSU_SPEC:-}" ]
then
    export SCALL="gosu ${GOSU_SPEC}"
fi

if [ -z "${GOSU_SPEC:-}" ]
then
    umask 000
fi

${SCALL} "$@"

EOF

