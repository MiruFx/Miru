<!-- 
Jobs
  Configuration
    Choosing a Storage Provider
      Same as {App}.Startup
      DbLite
    Services
      BackgroundServer should not be running

Asserting
  _.ShouldEnqueueOne<OrderCreatedJob>()
  var job = _.ShouldEnqueueOne<OrderCreatedJob>()
  job.OrderId.ShouldBe(456)
  _.ShouldEnqueueCount(1)
  
-->

[[toc]]

# Testing Jobs

