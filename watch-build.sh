#!/bin/bash

echo "🔍 Watching GitHub Actions build..."

# Get the latest workflow run ID for "Build Windows Executable"
RUN_ID=$(gh run list --workflow="Build Windows Executable" --limit=1 --json databaseId --jq '.[0].databaseId')

if [ -z "$RUN_ID" ]; then
    echo "❌ No workflow runs found"
    exit 1
fi

echo "📋 Watching run ID: $RUN_ID"
gh run watch $RUN_ID

# If successful, download the executable
if [ $? -eq 0 ]; then
    echo "✅ Build successful! Downloading executable..."
    mkdir -p ~/Sync
    gh run download $RUN_ID -D ~/Sync/CredBoard-Windows
    echo "📁 Executable downloaded to: ~/Sync/CredBoard-Windows/"
    echo "🎯 Ready to use: ~/Sync/CredBoard-Windows/CredBoard.exe"
else
    echo "❌ Build failed"
    exit 1
fi
