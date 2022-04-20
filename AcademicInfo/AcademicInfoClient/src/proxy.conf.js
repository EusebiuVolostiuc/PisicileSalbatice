const PROXY_CONFIG = [
  {
    context: [
      "/api/student",
      "/api"
    ],
    target: "https://localhost:7014",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
