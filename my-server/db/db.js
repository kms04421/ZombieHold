const mysql = require('mysql2');
// DB 연결 설정
const db = mysql.createConnection({
    host: 'localhost',
    user: 'root',
    password: '2188',
    database: 'test_db',
    connectionLimit: 10 // ?
});
// 연결 확인
db.connect(err => {
    if (err) {
        console.error('DB 연결 실패:', err);
        return;
    }
    console.log(' MySQL DB 연결 성공!');
});

module.exports = db;
