# Beginner Guide

So you own a Meta Quest (1, 2, or Pro) and you've never built and installed a Unity project on it? If yes, then follow the steps below!

## Verifying Your Oculus Developer Account and Creating an Organization

The first thing you need to do is verify your Oculus Developer Account. To do that, go to [this](https://developer.oculus.com/) website and log in with your Meta Account.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/login.png" width="600"/>
</p>

Next click on your profile picture and then click on `My Preferences`.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/preferences.png" width="600"/>
</p>

Next click on the `Verification` tab and verify your developer account by adding your credit card or setting up two-factor authentication.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/verification.png" width="600"/>
</p>

Next go to [this](https://developer.oculus.com/manage/organizations/create/) page to create an organization.

Enter any name you want, agree to the terms of service and press the Submit button. You will then see a developer NDA pop-up. Agree to it and press the Submit button.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/organization.png" width="600"/>
</p>

## Enabling the Developer Mode on Your Headset

When you first set up your headset, you must have been asked to download the Oculus app on a smartphone or tablet, and you must have synced your headset with that app.

Turn on your headset, open the Oculus app on your smartphone or tablet, select the `Menu` tab, and then select the `Devices` icon.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/menu.png" width="600"/>
</p>

Your headset should be displayed there, and its status should be `Connected`.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/devices.png" width="600"/>
</p>

Scroll down and click on `Developer Mode`.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/headset_settings.png" width="600"/>
</p>

Enable the `Developer Mode` checkbox.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/developer_mode.png" width="600"/>
</p>

## Verifying That the Developer Mode Is Enabled and Enabling Hand Tracking

Put your headset on and go to the `Settings` menu.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/settings.jpg" width="600"/>
</p>

Select the `System` box.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/system.jpg" width="600"/>
</p>

Look for the `Developer` menu. If it exists, then the Developer Mode is enabled, which is what we wanted.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/developer_mode_menu.jpg" width="600"/>
</p>

Go back to the `Settings` menu and select the `Movement Tracking` box.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/movement_tracking.jpg" width="600"/>
</p>

Enable the `Hand Tracking` checkbox.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/hand_tracking_menu.jpg" width="600"/>
</p>

## Installing the Oculus App on Your PC

Go to [this](https://www.meta.com/ca/quest/setup/?utm_source=www.meta.com&utm_medium=dollyredirect) website, download the Oculus App and install it.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/oculus_app.PNG" width="600"/>
</p>

Launch the Oculus App, go to the `Devices` tab and click on the `Add Headset` button.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/add_headset.PNG" width="600"/>
</p>

Follow the steps until your headset has been added.

## Installing the Meta Quest Developer Hub (MQDH) on Your PC

Go to [this](https://developer.oculus.com/documentation/unity/ts-odh/) website and follow the steps in the `Install MQDH` and `Connect Headset to MQDH` sections.

## Installing Unity 2022.1.23f1

Download the Unity Hub application from [this](https://unity.com/download) website if you don't have it installed already.

Launch it, go to the `Installs` tab and click the `Install Editor` button.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/unity_hub_installs.PNG" width="600"/>
</p>

Click on the `Archive` tab and then on the `download archive` link.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/unity_hub_archive.PNG" width="600"/>
</p>

Look for the correct version of Unity and click on the `Unity Hub` button.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/unity_archive_website.PNG" width="600"/>
</p>

Only select these modules and click the `Install` button.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/unity_hub_modules.PNG" width="600"/>
</p>

## Opening the Unity Project and Verifying That It Can Be Built and Installed on Your Headset

After cloning this repo, go to the `Projects` tab in the Unity Hub application, click on the arrow next to the `Open` button, and then click on the `Add project from disk` button.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/unity_hub_projects.png" width="600"/>
</p>

At this point it's very important that you don't choose the root directory of your clone of this repo. Instead, choose the `Handy` folder that's in the root directory.

For example, if you cloned this repo here: `C:\Users\JaneDoe\Desktop\handy`

Then select this folder: `C:\Users\JaneDoe\Desktop\handy\Handy`

You can now click on the project to open it:

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/unity_hub_open_project.PNG" width="600"/>
</p>

Next, plug your headset to your PC. Make sure to allow file access when prompted.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/access_files.jpg" width="600"/>
</p>

Within Unity, go to `Files -> Build Settings...`. Click on the Android icon and then on the `Switch Platform` button.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/switch_platform.PNG" width="600"/>
</p>

Next, click on the `Refresh` button and look for your device in the `Run Device` drop-down list. After selecting it, click on the `Build And Run` button. You will be asked what you want to name the `.apk` file that will be created. Choose any name. Once the build completes, put on your headset. The Handy application should launch automatically, and your hands should be tracked successfully.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/build_and_run.png" width="600"/>
</p>