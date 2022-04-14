const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
      "/api/student",
      "/api/staff",
      "/api/user"

    ],
    target: "https://localhost:7014",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
