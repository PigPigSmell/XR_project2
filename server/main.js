var net = require('net');

// creates the server
var server = net.createServer();

var clients = [];

const RolePool = {
    good: 0,
    bad: 0
};

const Game = {
    A: 0,
    B: 0,
    new: true
};

const Vote = {
    A: 0,
    B: 0,
};


//emitted when server closes ...not emitted until all connections closes.
server.on('close', function () {
    console.log('Server closed !');
});

// emitted when new client connects
server.on('connection', function (socket) {

    clients.push(socket);

    console.log('Buffer size : ' + socket.bufferSize);
    //var no_of_connections =  server.getConnections(); // sychronous version
    server.getConnections(function (error, count) {
        console.log('Number of concurrent connections to the server : ' + count);
    });

    socket.setEncoding('utf8');

    socket.setTimeout(800000, function () {
        console.log('Socket timed out');
    });


    socket.on('data', function (data) {
        data = data.trim();

        if (data == "connect" || data == "c") {
            if (RolePool.good <= RolePool.bad) {
                RolePool.good += 1;
                socket.write("role:good");
            } else {
                RolePool.bad += 1;
                socket.write("role:bad");
            }
        } else if (data == "start") { // we let game play
            clients.forEach(c => {
                c.write("scene start");
            })
        } else if (data[0] == "+") {
            if (Game.new) {
                setTimeout(end_game, 10000);
                Game.new = false;
                console.log('start new Game');
            }
            const num = Number(data.slice(1))
            Game.A += num;
        } else if (data[0] == "-") {
            if (Game.new) {
                setTimeout(end_game, 10000);
                Game.new = false;
                console.log('start new Game');
            }
            const num = Number(data.slice(1));
            Game.B += num;
        } else if (data[0] == "v" && data[1] == "o") {
            const who = data[5];
            if (who == 'A') {
                Vote.A += 1;
            } else {
                Vote.B += 1;
            }
            clients.forEach(c => {
                c.write(`A:B=${Vote.A}:${Vote.B}`);
            })
        } else if (data == "clear") {
            clients = [];

            RolePool.good = 0;
            RolePool.bad = 0;
            Vote.new = true;
            Vote.A = 0;
            Vote.B = 0;
            Game.A = 0;
            Game.B = 0;
        }

        function end_game() {
            const r = Game.A > Game.B ? "good" : "bad";
            const result = "winner:" + r;
            clients.forEach(c => {
                c.write(result);
            })

            Vote.new = true;
            Vote.A = 0;
            Vote.B = 0;

            // waiting 10 sec for voting
            setTimeout(end_vote, 10000);
            setTimeout(() => {
                Vote.A = 0;
                Vote.B = 0;
                console.log('clear Vote');
            }, 1000);


            // wait some time then clear data; avoid data racing
            setTimeout(() => {
                Game.new = true;
                Game.A = 0;
                Game.B = 0;
                console.log('clear Game');
            }, 5000);


        }

        function end_vote() {
            const r = Vote.A > Vote.B ? "A" : "B";
            const result = "go:" + r;
            clients.forEach(c => {
                c.write(result);
            })
            console.log('end Vote');
        }

        //echo data
        var is_kernel_buffer_full = socket.write('\nDEBUG: Data::' + data + '\n');
        if (is_kernel_buffer_full) {
            console.log('write successfully');
        } else {
            socket.pause();
        }

    });

    socket.on('drain', function () {
        console.log('write buffer is empty now .. u can resume the writable stream');
        socket.resume();
    });

    socket.on('error', function (error) {
        console.log('Error : ' + error);
    });

    socket.on('timeout', function () {
        console.log('Socket timed out !');
        socket.end('Timed out!');
        // can call socket.destroy() here too.
    });

    socket.on('end', function (data) {
        console.log('Socket ended from other end!');
        console.log('End data : ' + data);
    });

    socket.on('close', function (error) {
        console.log('Socket closed!');
        if (error) {
            console.log('Socket was closed coz of transmission error');
        }
    });

    setTimeout(function () {
        var isdestroyed = socket.destroyed;
        console.log('Socket destroyed:' + isdestroyed);
        socket.destroy();
    }, 1200000);

});

// emits when any error occurs -> calls closed event immediately after this.
server.on('error', function (error) {
    console.log('Error: ' + error);
});

//emits when server is bound with server.listen
server.on('listening', function () {
    console.log('Server is listening!');
});

server.maxConnections = 50;

//static port allocation
server.listen(2222);

var islistening = server.listening;

if (islistening) {
    console.log('Server is listening');
} else {
    console.log('Server is not listening');
}
setTimeout(function () {
    server.close();
}, 5000000);