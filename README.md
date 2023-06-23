# AiB-Breathing-Circle Repository
This repository was made by Max White. All scripts and assets in the repository were made by Max White with the help of beautiful people on Stack Overflow, Unity Forums, and Youtube. All scripts and assets are free to use in any of your projects at the Verse.
It is not required, but it would be greatly appreciated if you gave my credit somewhere in your project's credits if you use this code.

For any questions or concerns, you can reach me at: mjwhite2024@gmail.com

**About the Breathing Circle:**

The Breathing circle is a UI element that visualizes a breathing rate. By default, this breathing rate is: Breath in for 3 seconds, Hold peak of breath for 1 second, Breath out for 3 seconds, and Hold trough of breath for 1 sec, then repeat indefinitely. This default rate can be changed by going to Assets >> Scripts >> Rhythm_Behavior.cs >> and changing the values of in_time, peak_hold_time, out_time, and trough_hold_time.

These rates can be changed during gameplay via the pause menu. The pause menu can be accessed by pressing “esc” or “p” on the keyboard or pressing the “pause” button in the upper left hand side of the screen. The bindings for triggering a pause can be changed by going to Assets >> Input Folder >> Player_Input_Actions >> Pause >> and deleting or adding new bindings via the input system (Documentation for the Unity input system can be found here: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.6/manual/index.html)

**How the breathing circle works:**

*Rhythm_Behavior.cs:*

This script creates the breathing rhythm based off of the values held in in_time, peak_hold_time, out_time, and trough_hold_time.

*Inner_Circle_Behavior.cs:*

This script reacts to Rhythm_Behavior.cs by expanding the breathing circle whenever the rate breaths in and shrinking the breathing circle whenever the rate breaths out.

*Pause_Menu_Behavior.cs:*

This script reacts to player inputs in regards to pausing and unpausing. It will change Time.timeScale and set flags to be used in other scripts to achieve pausing behavior. If you add more scripts that use player input, make sure you follow the logic in line 25 of Breath_Handler.cs. Basically, make sure to include your input code within an if statement that reads as:

if(!Pause_Menu_Behavior.is_paused)

DO NOT FORGET the "!" at the start of the boolean!

*Slider_Behavior.cs:*

This script handles changes in the sliders that appear on screen when the game is paused. These sliders can be changed by the player to change the breathing rate. When the game unpauses, the breathing rate will always restart and begin to use whatever values are denoted by the sliders. By default, the sliders range from 1 to 10 and only allow for whole numbers. These preferences can be changed by going to: Hierarchy >> Canvas >> Pause_Menu >> Sliders >> and changing the settings of IN Slider, PEAK_HOLD slider, OUT slider, and TROUGH_HOLD slider via the Inspector.

*Breath_Handler.cs*

This script handles player inputs in regards to holding and releasing "space" on the keyboard to mimic breathing. 

*Boat_Behavior.cs*

This script allows the boat in the scene to move forward based Breath_Handler.cs and is kept for sake of showing an example of how Breath_Handler.cs can be used for gameplay. This script can be removed if you wish to make a different game using the breathing circle. If you do remove this script make sure to comment out line 62 in Breath_Handler.cs. This is the line that reads: 

boat_behavior.Move_Boat(breath_amount / Rhythm_Behavior.in_time);

**Boat example within the project:**

As previously mentioned, this project includes scripts and assets allowing for a boat to react to player input and the breathing circle. By holding "space" on the keyboard while the circle is expaning then releasing "space" while the circle is shrinking, the boat will move a distance forward. The more accuretly you hold and release in time with the breathing rhythm, the farther the boat will go. To remove this functionality, go to Assets >> Scripts >> Breath_Handler.cs >> and comment out line 62. This line is the line that reads:

boat_behavior.Move_Boat(breath_amount / Rhythm_Behavior.in_time);

