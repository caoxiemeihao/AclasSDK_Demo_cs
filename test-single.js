const path = require('path')
const cp = require('child_process')

const cmd = path.join(__dirname, 'AclasFor_node_SingleFile/bin/x86/Debug/AclasFor_node_SingleFile.exe')
const host = '192.168.1.3'
const filename = path.join(__dirname, './plu.txt')
const querystring = new URLSearchParams({
  host,
  filename,
  type: 0x0000, // type: 0x0000 PLU、0x0011 条码
}).toString()
const handle = cp.spawn(cmd, [querystring])

handle.stdout.on('data', chunk => {
  const str = chunk.toString()
  // console.log(str)
  const arr = parseData(str)

  console.log(arr)
})

function parseData(str) {
  let result = []
  // stdout 有两次 Console.WriteLine() 合并的情况，所以 res 是数组
  let res = String(str).trim().match(/(##(\w+)=(\{[\s\S][^##]+\})##)/g)
  if (Array.isArray(res)) {
    res.forEach(r => {
      const tmp = r.match(/^##(\w+)=(\{[\s\S]+\})##$/)
      if (Array.isArray(tmp)) {
        const cmd = tmp[1]
        let json = tmp[2]
        try {
          json = JSON.parse(json.replace(/\n/g, '<br/>'))
        } catch (error) {
          console.log()
          console.log((json))
          console.log('----')
          console.log(error)
          console.log()
          json = {}
        }
        result.push({ cmd, json })
      }
    })
  }

  return result
}
