# sbCurlClone

`sbCurlClone` is a command-line tool similar to `curl`, implemented in C#. It supports various HTTP methods, making it useful for interacting with RESTful APIs.

## Features

-   Supports HTTP methods: GET, POST, PUT, DELETE.
-   Enables sending custom headers.
-   Allows data sending with POST and PUT requests.
-   Verbose mode for detailed request and response headers.

## Usage

Navigate to the directory containing the `sbCurlClone` executable to execute these commands.

All examples are in cmd.

### Basic GET Request

```
sbCurlClone http://example.com
``` 

### Verbose Mode

View detailed request and response headers:

```
sbCurlClone -v http://example.com
``` 

### Specifying HTTP Method

Use `-X` followed by the method (GET, POST, PUT, DELETE):

```
sbCurlClone -X POST http://example.com
``` 

### Sending Data

For POST and PUT requests, use `-d` to send data:

```
sbCurlClone -X POST -d "{\"key\": \"value\"}" -H "Content-Type: application/json" http://example.com/post
``` 

### Custom Headers

Add custom headers with `-H`:

```
sbCurlClone -X POST -H "Content-Type: application/json" -d "{\"key\": \"value\"}" http://example.com/post
``` 

### DELETE Request

```
sbCurlClone -X DELETE http://example.com/resource
``` 

## Flags

-   `-v`: Verbose mode; prints request and response headers.
-   `-X`: Specifies the request method (GET, POST, PUT, DELETE).
-   `-d`: Sends specified data with the request. Useful for POST and PUT.
-   `-H`: Adds a custom header to the request.
