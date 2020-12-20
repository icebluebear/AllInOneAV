var app = require('express')();
var http = require('http').createServer(app);
var io = require('socket.io')(http);

app.get('/', (req, res) => {
  
});

io.on('connection', (socket) => {
  console.log('a user connected');
});

http.listen(30000, () => {
  console.log('listening on *:30000');
});

// var child = cp.execFile("G:\\Github\\AllInOneAV\\AllInOneAV\\GenerateReport\\bin\\Debug\\GenerateReport.exe",["report"], {maxBuffer: 1024 * 5000});

// child.stdout.on('data', function(data) {
//     console.log(data);
// });

// child.on('close', function() {
//     console.log('done');
// });