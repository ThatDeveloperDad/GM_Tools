# ThatDeveloperDad.Framework

## Purpose & Intent  
This project and its assembly will hold what I consider to be "utility" classes and functions for a variety of solutions I may build.

This means:  
1. There will be NO domain-specific logic in this assembly.
2. Inheritance hierarchies will be shallow, and define only those bits of functionality that must be consistent for ALL descendents.
3. We will organize it within namespaces that identify concepts and cross-cutting concerns.

> The classes and functions in this Framework are intended to be available to any component within our systems, regardless of archetype (Client, Manager, Engine, ResourceAccess.)