# Introduction
Tetris looked like the perfect game to test a powerful machine learning A.I. Because it is complex, but not too complex and it has an easy to understand ruleset. The goal of this project is to test the viability of machine learning in a really basic Tetris game.

# Design/implementation
## The making of my Tetris game
First thing on the to-do list Was to decide to use a game engine or to make it completely from scratch. I decided that using unity is the best option because it is easy to use and I can finish the game way faster in unity. To keep it simple I made block prefabs of all the Tetris blocks. Every block has a static grid member variable that hold all the current placed blocks. Then I made a block script that lowers them down. Once they reach the bottom they get added to the grid and are disabled. If a block moves down onto the grid it also get added to the grid and disabled. Then I made a spawner script that gets called by the block and spawns a new one every time one gets disabled and 1 time at the start of the game. Finally every time a block gets placed a check is run to see if there is a row. If there is a row it is deleted and all block above it are moved down.

![](/Images/tetrisWorks.gif)  

## Researching machine learning
After a bit of research I found a document [Here](https://melax.github.io/tetris/tetris.html) about using reinforcement learning for Tetris. After a few hours trying to understand it I still couldn't grasp anything useful written there so I kept looking. Then I found a unity package [Here](https://github.com/Unity-Technologies/ml-agents) that made implementing powerful machine learning agents in unity easier. Ml-agents can use 2 type of machine learning PPO(Proximal Policy Optimization) or SAC(Soft Actor Critic). I decided to use PPO. because PPO is more suited for situations where making substantial amounts of decisions continuously is necessary. SAC is more suited for boardgames with a slower pace. It is also less stable.

## implementing machine learning
To start implementing the A.I. I had to refactor some parts of the tetris code. I forgot to make a game over state. This is necesary because the ai needs to know when to restart. So I added a check to see if the grid grew to tall. I had to give some information to the ml-agent and reward the agent for good moves. To fix all of the above I added a game manager script. The manager script now holds the grid of blocks and the grid itself is no longer static to allow more than one game to run at once (for faster learning). I added a score system to the game manager script. The manager also gives all the needed information to the ml agent.

![](/Images/AIObservatiobs.png)  

This includes the grid of blocks and the current block. The game manager rewards the ml agent for "good moves" the bot is rewarded. A good move is decided by the moveScore. The moveScore gets increased by 10 if the placed block does not increase the total height of the grid. The moveScore also gets increased by 10 points if the placed block does not create new holes(an empty space with a block above it).

![](/Images/blockReward.png) 

The A.I. is also rewarded 1000 points for removing a row.

![](/Images/AIrewardRow.png)

The ml-agent returns 2 numbers that decide the actions it wants to take. The first is a number between 0 and 3: 0 do nothing, 1 move left, 2 move right and 3 rotate. The second number is always between 0 and 1. Like the first number 0 equals do nothing and number 1 increases the fallingspeed of the block.

![](/Images/agentMovement.png)

Now all of this is programmed the A.I. can almost start learning. the only thing left to do is make a config file.
Some extra information about the config options I used:
* trainer_type: Choose between PPO and SAC.
* summary_freq: Per how many steps data is printed to powershell(usefull to check if the A.I. is improving).
* max_steps: How many steps (updates of the ml-agent) before the program stops.
* time_horizon: how many steps 1 episode (one game of tetris) can be (the hnumber is large because tetris can go on for a long time).
* threaded: setting this to true allows the program to keep running while the A.I. calculates the next action.
* num_layers and hidden_units: Both affect the neural network the more complex the problem the higher they should be.
* vis_encoder_type: Used for images or very complex data. I set it to the default value.
* normalize: It might give undefined behaviour if set to false. Because of this it is recomended to be set to true.

![](/Images/AIconfig.png)

now the config file is done you can start the learning proces via powershell.

![](/Images/startTraining.png)

![](/Images/trainingRunning.png)

# Conclusion/Future work
This bot has trained 3 times for 20e6 steps. The result are disappointing because it seems like the A.I. didn't improve much.

 ![Result](/Images/aiNotSmart.gif)
 
I believe there are 2 main reasons the A.I. is not learning correctly. the first reason I think is the reward system, as the A.I. gets rewarded too often when making random moves and the reward for good moves is too small. The way to fix this is to add extra ways to reward the A.I. Maybe adding rewards for good moves with less inputs. The other porblem has to do with my implementation of tetris. There is a maximum amount of inputs/sec, because of this the A.I. gives inputs that do not get registered. This causes the A.I. to think it made moves that did not happen. To fix this I would have to refactor huge parts of my Tetris code. Ulitmatly, to make a better machine learning A.I. for Tetris, it would have been best if I made my own game from scratch without using an engine. This would have allowed me the freedom I needed to fix some core issues as the ones mentioned above.
