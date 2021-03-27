# FTP Smart File Copy
This .NET-based GitHub Action updates the destination to match the source over FTP, by executing the following steps:
- Delete files from destination that do not exist in source.
- Update files that have been modified in source since they were last modified in destination.
- Upload files from source that do not exist in destination.

## Inputs
| Parameter   | Required  | Default | Description                                  |
| ----------- | --------- | ------- | -------------------------------------------- |
| server      | **Yes**   |         | Address for the destination server.          |
| port        | No        | **21**  | Port for the destination server.             |
| username    | **Yes**   |         | Username for the destination server.         |
| password    | **Yes**   |         | Password for the destination server.         |
| source      | No        | **/**   | Directory in source from which to upload.    |
| destination | No        | **/**   | Directory in destination to which to upload. |

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
      uses: actions/checkout@v2.3.2
      
    - name: FTP Deploy
      uses: cinderblockgames/ftp-action@main
      with:
        # required
        server: ftp.example.com
        username: example@example.com
        password: ${{ secrets.FTP_PASSWORD }}
        # optional
        port: 22
        source: /src/
        destination: /target/
```
