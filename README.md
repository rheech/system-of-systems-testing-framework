# System of Systems Testing Framework

System of Systems Testing (SoST) Framework is a simulation-based testing tool of a Goal-Oriented Testing Approach for System of Systems. The tool is composed of Goal-Oriented Test Oracle (GOTO) Generator and a simulator to support the test oracle. Please refer to the related paper for more details.

The source code is published under MIT License.

## Screenshot
![Screenshot](/doc/screenshot.png)

## Version
1.1.0

## Prerequisites
* Windows 7 SP1 x64 (Windows-based operating system with .NET Framework)
* Visual Studio 2010 SP1

## Projects in SoST Framework Solution
01_Main
* SoS_Simulator
  * A simulation engine to execute the system of systems prototype.
* TestOracleGenerator
  * A Goal-Oriented Test Oracle (GOTO) Generator library that takes xml-based oracle as an input and pulls out the test oracle.

02_Scenario
* Scenario_Prototype
  * SoS Prototype library
* Utility_Prototype
  * Utility function based test oracle library

## Run Instruction
1. Open SoS prototype library (.dll) file of your choice by clicking Open button.
2. Open the corresponding Goal-Oriented Test Oracle (GOTO) in XML file by clicking Browse button.
3. Run the simulator.

## Implemented Prototypes
* Mass Casualty Incident (MCI)
* E-Commerce
* Mass Casualty Incident (MCI) Single
* Smart Home System (partially developed)

## Related Papers
Cheonghyun Lee, "A Goal-Oriented Testing Approach for System of Systems Using Task Models", Master Thesis, Korea Advanced Institute of Science and Technology, 2016