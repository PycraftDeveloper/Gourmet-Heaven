<div align="center">

  ![Gourmet Heaven logo](https://github.com/user-attachments/assets/3c4d1d82-a489-4f87-bffe-0e74491ac19d)
</div>

# Gourmet Heaven

Welcome to our game.

## Unity Setup Guide

1. Clone the repository from here to a location on your PC.
2. Open Unity Hub.
3. On the main page click the button that says "**Add**".
4. Click "**Add project from disk**".
5. Locate the place you cloned the GitHub repo - we are **NOT** done yet.
6. Within the folder there should be another folder called "**GroupProject**".
7. This is where you want Unity to recognize this project.
8. You are now 'done', however you will also likely need to check out the `Project Resources Guide` below.

> **CAUTION**: If you open GitHub and it says that the number of files to commit is large (>1000 files) then double check you got the location right. If you are unsure, please consult Jebbo.

## Upgrading your version of Unity

You likely started working on the project using Unity version 6000.0.5f1. However, because this version apparently has problems with Android compilation, we have switched to Unity version 6000.0.39f1. If you already have installed the old version, this is how you can update your unity version!

1. Go [here](unityhub://6000.0.39f1/15ea7ed0b100) to download the right version of Unity. Alternatively go [here](https://unity.com/releases/editor/archive) and find the right version manually.
2. You should see Unity Hub prompt you to go through the installation process.
3. In the menu that allows you to pick what platforms you want, make sure to select: **Android Build Support**,all it's sub-options, as well as **iOS Build Support**.
4. Wait for Unity Hub to install this version.
5. On the **Projects** page on Unity Hub, click the editor version, and change it to the version you just installed.
6. Open the Unity project and let it do its thing.
7. Once open, double check on GitHub that you don't have a massive number of files changed (>100).
8. If done correctly, you should be good to go!

> **CAUTION**: DO NOT continue using Unity version 6000.0.5f1 when working on the project. You **MUST** update before continuing with development.

_Note: The additional platforms (Android Build Support and iOS Build Support) may be optional, but I'd recommend installing them because we will be using these components when building the project for these platforms._

_Note: If you want to build the project to an APK file (android build) and get errors with the Android SDK, you might need to completely uninstall and reinstall the editor and do everything again (sometimes many, many times). Before trying that, double check with Jebbo!_
