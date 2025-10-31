const { WebSocketServer } = require('ws');
function initWebSocket(server) {
    const wss = new WebSocketServer({ server });
    console.log('WebSocket 서버 실행됨');

    const zombies = {};
    const players = {};

    const { v4: uuidv4 } = require('uuid');

    wss.on('connection', (ws) => {
        const playerId = uuidv4();
    

        console.log(`플레이어 접속: ${playerId}`);
        const newPlayer = {
            id: playerId,
            position: { x: 0, y: 0, z: 0 },
        }
        const spawnMsg = JSON.stringify({
            type: "NewPlayer",
            data: newPlayer
        });

        wss.clients.forEach(client => {
            if (client.readyState === 1) {
                client.send(spawnMsg);
            }
        });
   
        ws.send(JSON.stringify({ type: "AssingID", id: playerId })); //본인 id전송

        ws.on('message', (msg) => {
            const data = JSON.parse(msg);

            switch (data.type) {
            
                    //여기서부터 좀비
                case 'registerZombie':
                    zombies[data.id] = {
                        hp: data.hp,
                        template: data.template // 나중에 템플릿/스킬 정보도 저장 가능
                    };
                    console.log(`Zombie registered: ID=${data.id}, HP=${data.hp}`);
                    break;

                case 'damageZombie':
                    const z = zombies[data.id];
                    if (z) {
                        z.hp -= data.damage;
                        const isDead = z.hp <= 0;

                        const hitMsg = JSON.stringify({
                            type: 'zombieHit',
                            id: data.id,
                            hp: z.hp > 0 ? z.hp : 0,
                            dead: isDead
                        });

                        // 모든 클라이언트로 전송
                        wss.clients.forEach(client => {
                            if (client.readyState === 1) client.send(hitMsg);
                        });

                        if (isDead) delete zombies[data.id];
                    }
                    break;

                    // 여기서 부터 플레이어
                case 'registerPlayer':
                    players[data.id] = {
                        currentHP: data.currentHP,
                        maxHP: data.maxHP,

                    };
                    console.log(`Player registered: ID=${data.id}, MaxHP=${data.MaxHP}`);
                    break;

                case 'damagePlayer':
                    const p = players[data.id];
                    if (p) {
                        p.currentHP -= data.damage;
                        const isDead = p.currentHP <= 0;

                        const hitMsg = JSON.stringify({
                            type: 'playerHit',
                            id: data.id,
                            currentHP: p.currentHP > 0 ? p.currentHP : 0
                        });

                        wss.client.forEach(client => {
                            if (client.readyState === 1) client.send(hitMsg);
                        });                    
                    }
                    break;

                case 'playerUpdate':
                    players[data.id] = data;
                    break;

                case 'nightStart':
                    const zombieCount = Math.floor(Math.random() * 5) + 5;
                    const spawnMsg = JSON.stringify({ type: 'spawnZombie', count: zombieCount });

                    wss.clients.forEach(client => {
                        if (client.readyState === 1) client.send(spawnMsg);
                    });
                    break;
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