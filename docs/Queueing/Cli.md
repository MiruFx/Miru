# Queueing Cli Commands

## Running Queues

### Running all queues

```shell
miru queue.run 
```

### Setting amount of workers per queue

```shell
miru queue.run --queues default:5 email:1
```

The `default` queue will have 5 workers to process jobs and the `email` queue will have 1 worker

## Cleaning Queues

```shell
miru queue.clear
```
