# FTP Smart File Copy
This .NET-based GitHub Action updates the destination to match the source over FTP, by executing the following steps:
- Delete files from destination that do not exist in source.
- skipUnchanged?
  - true
    - Update files that do not match between source and destination.
    - Upload files from source that do not exist in destination.
  - false
    - Upload all files from source to destination.

## Inputs
| Parameter       | Required  | Default                  | Description                                                                               |
| :---            | :---      | :---                     | :---                                                                                      |
| **server**      | **Yes**   |                          | Address for the destination server.                                                       |
| port            | No        | **21**                   | Port for the destination server.                                                          |
| **username**    | **Yes**   |                          | Username for the destination server.                                                      |
| **password**    | **Yes**   |                          | Password for the destination server.                                                      |
| source          | No        | **/**                    | Directory in source from which to upload.                                                 |
| destination     | No        | **/**                    | Directory in destination to which to upload.                                              |
| skipUnchanged   | No        | **false**                | Only upload files that have changed.                                                      |
| skipDirectories | No        | **.github\|.well-known** | Folders to be ignored in both source and destination, separated by a pipe (\|) character. |
| test            | No        | **false**                | Do not perform file deletions or uploads; just output intended actions.                   |

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
      uses: cinderblockgames/ftp-action@v1.2.0
      with:
        # required
        server: ftp.example.com
        username: example@example.com
        password: ${{ secrets.FTP_PASSWORD }}
        # optional
        port: 22
        source: src/path
        destination: target/path
        skipUnchanged: true
        skipDirectories: .github|.well-known|configs|private-keys
        test: true
```
