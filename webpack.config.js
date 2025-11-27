module.exports = {
    target: "node",
    devServer: {
        // ... other devServer options
        setupMiddlewares: (middlewares, devServer) => {
            // Your middleware logic for after setup
            // Example: middleware1(devServer.app);
            // Example: middleware2(devServer.app);

            // If you had onBeforeSetupMiddleware, add that logic here as well
            // Example: beforeMiddleware1(devServer.app);

            return middlewares;
        },
    },
    resolve: {
        fallback: {
            "zlib": require.resolve("browserify-zlib"),
            "querystring": false,
            "crypto": require.resolve("crypto-browserify"),
            "stream": require.resolve("stream-browserify"),
            "timers": require.resolve("timers-browserify"),
            // Add fallbacks for other Node.js modules if needed (e.g., "crypto", "stream", "http")
        }
    }
}