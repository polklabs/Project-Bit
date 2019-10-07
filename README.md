# Project-Bit - A Logic Circuit Simulator

The goal of this project is to create a logic simulator that is accurate enough to build a working 8 bit computer that runs in near realtime. The components available in the simulator should be similar to components you would be able to buy in real life.

### Inspired By
https://eater.net/8bit

## Limitations
1. Does not simulate - voltage, resistance, current
2. Ignores gate propagation delay
3. Application runs at 60fps, any updates faster than that may not function properly
4. Switches are one way gates. This can be changed to work in both directions but would most likely harm performance 
(Further testing needed)
5. Gates and Chips are ideal aproximations and will most likely behave differently in real life

## Components
#### Gates
The basic building blocks (And, Or, Xor...)
#### Chips
Logic grouping made up of gates and or other chips
#### Components
Contains chips/gates or custom logic
