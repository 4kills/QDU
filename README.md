# QDU 

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

QDU is an ultra lightweight screenshot tool that allows quick and easy sharing of screenshots across the web!  
You can either copy them to your clipboard and share from there, save to disk, or get a sharable link by uploading the screenshot to a [QDU server](https://github.com/4kills/qdu_server).

The latter can be hosted by you, a friend, or someone else in the community! However, QDU is also completely functional in an offline manner, without uploading any data.

**If you consider using the online option, please read the [Privacy and Security Notice](#privacy-and-security-notice) first**. 

## Table of Contents

- [Features](#features)
- [Getting Started](#getting-started)
  - [Installation](#installation)
  - [Connecting to a QDU Server (optional)](#connecting-to-a-qdu-server-optional)
- [Usage](#usage)
- [Privacy and Security Notice](#privacy-and-security-notice)
- [Notes](#notes)
- [License](#license)

# Features

- [x] Screenshot area or all screens
- [x] Save screenshot to clipboard, disk, webserver
- [x] View gallery of uploaded screenshots
- [x] Customizable hotkeys for every action 
- [x] Easily toggable autostart
- [x] Clean and minimalistic interface
- [x] Super lightweight: Minimal memory footprint and idle cpu time

# Getting Started

## Installation 

Just download the latest executable from [here](https://github.com/4kills/QDU/releases) or clone the repository and compile it yourself!  

QDU requires no installer and can be used right from the .exe. 

After launching the application it can be found in your system tray and be configured from there. 

## Connecting to a QDU Server (optional)

In order to connect and upload to a [QDU server](https://github.com/4kills/qdu_server), you need the address of someone hosting [it](https://github.com/4kills/qdu_server). 

In the options panel (accessible from the tray icon) you may enter an URL or directly a (public) IP address to the server. You may also enter a port, if your remote server is not listening to the default 1337 port. Lastly, you only need to switch the `Mode` to `Online`. 

**If you consider using the online option, please read the [Privacy and Security Notice](#privacy-and-security-notice) first**.

# Usage

QDU was primarily designed to be used with hotkeys which can be set under Options > Keybinds. The Options can be accessed by right-clicking the tray icon. 

Hotkeys allow for a quicker usage but are not necessary - screenshots can also be taken using the tray icon. 

Once the hotkeys are set, you can press them to activate taking a screenshot from anywhere and of anything you like! For the area screenshot just press your hotkey and left-click drag the area you want to screenshot. 
Immediately after releasing the mouse button the screenshot will be saved to your clipboard, your disk, or the connected webserver, which will then return a link to the screenshot to your clipboard which may be pasted anywhere! 

The `Mode` of saving can be configured in the `Options` panel: 

- `Save to Disk`: Your file browser will open to prompt you to choose a location for every screenshot. You can circumvent this hassle by checking `Remember my Location`, which will then save all subsequent screenshots to the last chosen location. 

- `Save to Clipboard`: Upon taking a screenshot this screenshot will be saved to your clipboard, which can then be pasted to software supporting pictures from clipboards (like Discord, Skype, WhatsApp Web, etc...). 

- `Online`: Upon taking a screenshot that screenshot will be uploaded to the web server, which will then host the picture and return a link to it. This link will be accessible from your clipboard, so it can be pasted to every text input field. Everyone with the link can view the picture.  

# Privacy and Security Notice

If you use the `Online` mode, the sceenshots you take will be uploaded to a remote server. **Be sure you trust the remote address.** As the pictures are **not** encrypted, nor otherwise secured. This means the server admins can view **all of your uploads**.

Additionally, **everyone with the link can view the picture**. So be sure not to post any sensitive content/information, that is not intended for everyone on the Internet. Remember, the Internet can be a scary place and the Internet never forgets. 

Anyhow, people with a link to one of your pictures will not be able to view any other pictures of yours. Except for server admins, it is not possible to connect a picture to a certain user. 

The (**unmodified!**) QDU Servers do not store any data of you, except an anonymous account token, your pictures, and the connection of your account token to a picture. 

Your `Gallery` is tied to your account token, so if you share your Gallery or your account token, everyone with the link will be able to see all of your past and future screenshots! **DO NOT SHARE YOUR GALLERY OR ACCOUNT TOKEN**, except you are absolutely trusting the recipient! 

The creators of this software, especially but not exclusively the copyright holders mentioned in the [License](#license), **may never** be held liable, nor responsible for any damages arising from connecting to remote servers **not operated** by the aforementioned creators. If you have any privacy concerns, please contact the server operators directly. 

# Notes

- project started on November 24, 2017

# License

```txt
MIT License

Copyright (c) 2020 Dominik Ochs

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
