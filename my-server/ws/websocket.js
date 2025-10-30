const { WebSocketServer } = require('ws');
function initWebSocket(server) {
    const wss = new WebSocketServer({ server });
    console.log('WebSocket 서버 실행됨');

    const zombies = {};
    const players = {};

    wss.on('connection', (ws) => {
        console.log('클라이언트 연결됨');

        ws.on('message', (msg) => {
            const data = JSON.parse(msg);

            // 플레이어 이동/행동
            if (data.type === 'playerUpdate') players[data.id] = data;

            // 좀비 피해 처리
            if (data.type === 'damageZombie') {
                const z = zombies[data.zombieId];
                if (z) {
                    z.hp -= data.damage;
                    if (z.hp <= 0) delete zombies[data.zombieId];
                }
            }

            // 밤 시작 -> 좀비 소환 예시
            if (data.type === 'nightStart') {
                const zombieCount = Math.floor(Math.random() * 5) + 5;
                const spawnMsg = JSON.stringify({ type: 'spawnZombie', count: zombieCount });

                wss.clients.forEach(client => {
                    if (client.readyState === 1) client.send(spawnMsg);
                });
            }
        });

        ws.on('close', () => console.log('클라이언트 연결 종료'));
    });

    // 100ms 마다 상태 브로드캐스트
    setInterval(() => {
        const state = { players, zombies };
        const msg = JSON.stringify({ type: 'stateUpdate', data: state });

        wss.clients.forEach(client => {
            if (client.readyState === 1) client.send(msg);
        });
    }, 100);
}

module.exports = initWebSocket;