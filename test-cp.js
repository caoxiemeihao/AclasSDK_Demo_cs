const http = require('http');
const utils = require('util');
const os = require('os');
const fs = require('fs');
const path = require('path');
const cp = require('child_process');

function query(params, port = '9999') {
  return new Promise(resolve => {
    http.get(`http://127.0.0.1:${port}/?${new URLSearchParams(params).toString()}`,
      res => {
        res.on('data', chunk => {
          let str = chunk.toString();
          try {
            str = JSON.parse(str);
          } catch (error) {
            console.log();
            console.log(str);
            console.log('----');
            console.log(error);
            console.log();
          }
          resolve(str);
        });
      })
  });
}

const handle = cp.spawn(path.join(__dirname, 'AclasFor_node/bin/x86/Debug/AclasFor_node'));
handle.stdout.on('data', function (chunk) {
  const str = chunk.toString();
  // stdout 有两次 Console.WriteLine() 合并的情况，所以 res 是数组
  const res = String(str).trim().match(/(##(\w+)=(\{[\s\S][^##]+\})##)/g);
  if (Array.isArray(res)) {
    res.forEach(r => {
      const tmp = r.match(/^##(\w+)=(\{[\s\S]+\})##$/)
      const cmd = tmp[1];
      let json = tmp[2];
      try {
        json = JSON.parse(json);
      } catch (error) {
        console.log();
        console.log(json);
        console.log('----');
        console.log(error);
        console.log();
      }
      if (cmd === 'server' && json.state === 'start') {
        query({
          cmd: 'start',
          host: '192.168.1.3',
          // file: path.join(os.homedir(), 'Desktop/plu.txt'),
          file: path.join(__dirname, './plu.txt'),
        }, json.port).then(json => {
          console.log('[Query response]', json);
        });
      } else if (cmd === 'start') {
        console.log('开始下发', json);
      } else if (cmd === 'dispatch') {
        let msg = '';
        if (json.code === 0) {
          msg = '完成';
          handle.kill();
        } else if (json.code === 1) {
          msg = '下发中';
        } else {
          msg = '报错';
          handle.kill();
        }
        console.log(msg, json);
      }
    });
  }
});
