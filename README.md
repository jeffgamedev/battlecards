# Battle Cards Demo

Created by jeffgamedev aka Jeff Brooks 9/9/20.

Created with Unity 2020.1.3f1 https://unity.com/

Clone the repository to your local machine, install Unity 2020.13f1+, and import the project via the Unity Hub.

Background by jkjkke https://opengameart.org/content/background-5

Weapon Icons by Scrittl https://opengameart.org/content/weapon-icons-32x32px-painterly

Portraits by CobraLad https://opengameart.org/content/32x32-fantasy-portrait-set

Card by Searle https://opengameart.org/content/card-template

Followup Questions:

  * How would your code change if weapons had range?
  Depending on how it would be desired for range to be implemented, range could have a multitude of changes on the code. In the case of this project, x and y coordinates are never taken into account, and I imagine Range would ultimately affect the attack time and accuracy of an attack.
  
  * How would your code change if weapons had special effects, like the ability to make targets catch fire? I would create status effect data type, and allow weapons to have this effect by an id. When a character make's its attack, it would add the status effect to the targeted character, where it would be managed.
  
  * How might this system be incorporated into a larger items and inventory system? Likely, there would be an inventory management controller which ties directly to the loaded JSON data. I would add identifiers to each item so they can be managed and referenced easier.
