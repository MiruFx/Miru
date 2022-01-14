<!--
good tests:
    - easily readable and understand the case/scenario
    - arrange and assert only relevant for the case/scenario

arrange, act, assert

one arrange per class

arrange
    'entities then request' pattern
        create all the entities needed
        fill the request with ids needed for the scenario

assert
    list assert count, first, and last
        ShouldCount
        First()
        Last()

test saving new entity
    .LastSaved()

test saving existent entity
    .LastSaved()

-->

# Feature Test Patterns

