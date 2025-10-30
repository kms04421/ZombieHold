const express = require('express');
const cors = require('cors');
const http = require('http');

const dataRoutes = require('./routes/data');
const zombieRoutes = require('./routes/zombie');
const itemRoutes = require('./routes/item');
const abilityRoutes = require('./routes/ability');
const initWebSocket = require('./ws/websocket');

const app = express();
const PORT = 3000;

// 미들웨어
app.use(cors());
app.use(express.json());

// 라우트 등록
app.use('/zombie', zombieRoutes);
app.use('/item', itemRoutes);
app.use('/ability', abilityRoutes);
app.use('/', dataRoutes);

// HTTP + WebSocket 서버 생성
const server = http.createServer(app);
initWebSocket(server);

server.listen(PORT, () => {
    console.log(`서버 실행 중: http://localhost:${PORT}`);
});