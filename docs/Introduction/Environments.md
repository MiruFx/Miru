<!--
Configuration
TODO: Command Line args
TODO: Environment variables
Config.yml
  TODO: same as appconfig.json
-->

# Environments

By default Miru expects three environments:

* Development: where developers code and manual test features development
* Test: where automated tests are run. Can be on developers and testers machine or continuous integration machine.
* Production: where the application runs

Depending on your team and needs, we recommend adding more two environments in your process:

* Validation: where testers can manually verify if the application works as expected
* Staging: same configuration in production and allows the team verify if the application works as expected before deploying into production

# Recommend Configuration

| Environment   | Topic        | Configuration                                                                                                                                                                                              |
|---------------|--------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `Development` | RecurringJob | `recurrent` queue's worker not enabled. Jobs should be enqueued but not run so it doesn't disturb debugging. To process the enqueued recurrent jobs run the `recurrent` queue manually through `queue.run` |
|               | Queueing     | Queue's worker enabled with only one worker. More than one worker can make debugging difficult                                                                                                             |




