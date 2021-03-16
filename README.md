# SharpRPC

## Installation

### Installation is very simple as long as you read all instructions

### Windows

First, I must mention that if you are not running 64-bit windows, you must not download the `win-x64` because it won't work. Find out if your windows version is [64-bit or 32-bit](https://www.computerhope.com/issues/ch001121.htm#:~:text=Press%20and%20hold%20the%20Windows,running%20the%2064-bit%20version.).

#### 32-bit

Download `win-x86.zip`, unzip and extract the folder inside.

#### 64-bit

You can choose either `win-x86.zip` or `win-x64.zip`. It is recommended you choose `win-x64.zip`. The steps from there are the same: unzip the folder and extract the folder inside.

### macOS

Download `macOS.dmg`, open it, and extract the folder inside.

### Linux

Download `linux-x64.zip`, open it, and extract the folder inside. You must make sure your Linux distribution is 64-bit.

## General Setup

Inside the folder with the application files, create a new file called `config.json`. Then, you can fill in the values according to the chart below:

| key                 | value                                    | default | required |
| ------------------- | ---------------------------------------- | ------- | -------- |
| `applicationID`     | discord application id (text)            | None    | Yes      |
| `applicationSecret` | discord applicaiton secret (text)        | None    | Yes      |
| `updateInterval`    | time between updates (in seconds number) | `10`    | No       |
| `statusList`        | list of status objects (see below)       | None    | Yes      |

### Status Object

| key          | value                             | default | required |
| ------------ | --------------------------------- | ------- | -------- |
| `details`    | top presence text (text)          | None    | No       |
| `state`      | bottom presence text (text)       | None    | No       |
| `largeImage` | presence image object (see below) | None    | No       |
| `smallImage` | presence image object             | None    | No       |

### Presence Image Object

| key    | value                     | default | required |
| ------ | ------------------------- | ------- | -------- |
| `key`  | asset name (text)         | None    | No       |
| `text` | the text you see on hover | None    | No       |

## Example config.json

```json
{
  "statusList": [
    {
      "details": "SharpRPC Default",
      "state": "Waiting for new RPC",
      "largeImage": {
        "key": "sharpcordgrey"
      },
      "smallImage": {
        "key": "csharp",
        "text": "Made by Constanze#1337"
      }
    }
  ],
  "applicationID": "821204366966784021",
  "applicationSecret": "<secret>",
  "updateInterval": 20
}
```
