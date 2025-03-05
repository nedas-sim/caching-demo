# caching-demo

## Load tests

To run load tests:

- Install and run Docker
- Install k6
- Run DataStoreDemo.AppHost
- Use these commands to run scripts:
  - k6 run load-test-database.js
  - k6 run load-test-cache.js
  - k6 run load-test-output-cache.js
