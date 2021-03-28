# FTP Smart File Copy
This .NET-based GitHub Action updates the destination to match the source over FTP, by executing the following steps:
- Delete files from destination that do not exist in source.
- ignoreUnchanged?
  - true
    - Update files that have been modified in source since they were last modified in destination.
    - Upload files from source that do not exist in destination.
  - false
    - Upload all files from source to destination.

## Inputs
| Parameter       | Required  | Default     | Description                                                                                     |
| --------------- | --------- | ----------- | ----------------------------------------------------------------------------------------------- |
| server          | **Yes**   |             | Address for the destination server.                                                             |
| port            | No        | **21**      | Port for the destination server.                                                                |
| username        | **Yes**   |             | Username for the destination server.                                                            |
| password        | **Yes**   |             | Password for the destination server.                                                            |
| source          | No        | **/**       | Directory in source from which to upload.                                                       |
| destination     | No        | **/**       | Directory in destination to which to upload.                                                    |
| ignoreUnchanged | No        | **false**   | Do not upload any file that hasn't changed.  Setting to true will be slower than leaving false. |

## Example Workflow
```
# Workflow name
name: Deploy site to live
 
on:
  # Run automatically on push to main branch
  push:
    branches: [ main ]
    paths:
    - 'src/**'
  # Allow manual trigger
  workflow_dispatch:

jobs:
  web-deploy:
    name: Deploy
    runs-on: ubuntu-latest
    steps:
    - name: Get files
      uses: actions/checkout@v2.3.4
      
    - name: FTP Deploy
      uses: cinderblockgames/ftp-action@v1.0.1
      with:
        # required
        server: ftp.example.com
        username: example@example.com
        password: ${{ secrets.FTP_PASSWORD }}
        # optional
        port: 22
        source: src/path
        destination: target/path
        ignoreUnchanged: true
```
