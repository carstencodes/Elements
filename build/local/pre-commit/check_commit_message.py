#!/usr/bin/env python3
import os
import re
import sys
from pathlib import Path


COMMIT_MSG_FILE = Path(sys.argv[1]) if len(sys.argv) > 1 else None

if COMMIT_MSG_FILE is None:
    sys.exit(0)

if not COMMIT_MSG_FILE.exists():
    sys.exit(0)

message = COMMIT_MSG_FILE.read_text(encoding="utf-8").strip()
if not message:
    sys.exit(0)

pattern = re.compile(
    r"^(build|chore|ci|docs|feat|fix|perf|refactor|revert|style|test)(\([a-zA-Z0-9._-]+\))?!?: .+"
)

if not pattern.match(message):
    print("Commit message must follow Conventional Commits format.")
    print("Expected: <type>[optional scope]: <subject>")
    print("Example: feat(api): add new endpoint")
    sys.exit(1)

print("Conventional commit message detected.")
