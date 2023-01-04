# handy

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/logo.PNG" width="600"/>
</p>

The easiest way to mocap your hands!


## Background

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/animated_hands.gif" width="600"/>
 <p align="center">
  <em>No artists had to suffer to make this Blender animation. It was all mocapped using a Meta Quest Pro!</em>
 </p>
</p>

We developed this tool in order to streamline the process of capturing hand movements from VR devices and bringing them into Blender for use in animations.


## Workflow

1. First, build the `CaptureScene` and install it to the VR device
2. Run the `CaptureScene` on the VR device
3. Start and stop recording by pinching your left thumb and index fingers together and holding it until the red recording indicator appears or disappears
4. Attach the VR device and download the `.jsonlines` files that were recorded
5. Load `PlaybackScene` in the editor
6. Click on `PlaybackManager` in the scene hierarchy and input the name of the `.jsonlines` file that you want to export to Blender
7. Hit play in the editor and wait for the animation to complete
8. Load your exported .abc file in Blender!


## Prerequisites
* Unity 2022.1.23 or later
* Meta Quest (1, 2, or Pro)


## License

This project is licensed under the MIT License - see the LICENSE file for details.