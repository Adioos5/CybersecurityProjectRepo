const express = require('express')
const app = express()
const port = 3000

const serveFiles = express.static('./out')
app.use(serveFiles)

app.listen(port, () => {
  console.log(`App listening on port ${port}`)
})

