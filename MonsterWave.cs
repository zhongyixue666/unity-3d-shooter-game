using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Suriyun.MobileTPS
{
    // 怪物波次类，管理怪物的生成和波次的开始、停止、重启
    public class MonsterWave : MonoBehaviour
    {
        // 波次类型，主波次或子波次
        public WaveType wave_type = WaveType.MainWave;
         public MonsterWave monsterWave;
        // 怪物预制体
        public GameObject mob_prefab;
        // 要生成的怪物总数
        public int total_mobs_to_release;
        // 生成前的延迟时间
        public float delay_before_spawn;
        // 生成间隔时间
        public float spawn_interval = 1f;
        // 怪物生成点列表
        public List<Transform> mob_spawn_point;
        // 是否正在生成怪物
        public bool is_spawning = false;

        // 子波次列表
        public List<MonsterWave> sub_waves;

        // 已生成的怪物列表
        [HideInInspector]
        public List<Enemy> mobs;
        // 已释放的怪物数量
        [HideInInspector]
        protected int mobs_released;
        // 生成协程
        [HideInInspector]
        protected Coroutine c_spawner;

        // 开始波次
        public virtual void StartWave()
        {
            // 移除所有空的子波次
            for (int i = 0; i < sub_waves.Count; i++)
            {
                if (sub_waves[i] == null)
                {
                    sub_waves.RemoveAt(i);
                    i--;
                }
            }

            // 开始所有子波次
            foreach (MonsterWave sw in sub_waves)
            {
                sw.wave_type = WaveType.SubWave;
                sw.StartWave();
            }

            // 将自身添加到子波次列表
            this.sub_waves.Add(this);
            // 启动生成协程
            c_spawner = StartCoroutine(Spawner());
            // 触发波次开始事件
            this.EventWaveStart.Invoke();
            
        }

        // 停止波次
        public virtual void StopWave()
        {
            // 从子波次列表中移除自身
            this.sub_waves.Remove(this);
            // 重置已释放的怪物数量
            this.mobs_released = 0;
            // 设置生成状态为false
            this.is_spawning = false;

            // 停止生成协程
            if (c_spawner != null)
            {
                this.StopCoroutine(c_spawner);
                c_spawner = null;
            }

            // 停止所有子波次
            foreach (MonsterWave sw in sub_waves)
            {
                if (sw != this)
                {
                    sw.StopWave();
                }
            }
            // 触发波次停止事件
            this.EventWaveStop.Invoke();
        }

        // 重启波次
        public virtual void RestartWave()
        {
            // 销毁所有子波次中的怪物
            foreach (MonsterWave w in sub_waves)
            {
                for (int i = 0; i < w.mobs.Count; i++)
                {
                    if (w.mobs[i] != null)
                    {
                        //Destroy(w.mobs[i].gameObject);
                        w.mobs[i].ForceDie();
                    }
                    w.mobs.RemoveAt(i);
                    i--;
                }
            }

            // 停止波次
            this.StopWave();
            // 开始波次
            this.StartWave();
        }

        // 生成怪物的协程
        protected virtual IEnumerator Spawner()
        {
            // 等待生成前的延迟时间
            yield return new WaitForSeconds(delay_before_spawn);
            // 触发延迟结束事件
            this.EventDelayEnded.Invoke();
            // 设置生成状态为true
            this.is_spawning = true;

            // 循环生成怪物，直到达到总数或停止生成
            while (is_spawning && mobs_released < total_mobs_to_release)
            {
                // 随机选择一个生成点
                int rand = UnityEngine.Random.Range(0, this.mob_spawn_point.Count - 1);
                // 实例化怪物预制体
                GameObject g = (GameObject)Instantiate(mob_prefab);
                // 设置怪物的父对象为null
                g.transform.parent = null;
                // 设置怪物的位置为生成点的位置
                g.transform.position = this.mob_spawn_point[rand].position;

                // 将怪物添加到已生成的怪物列表
                mobs.Add(g.GetComponent<Enemy>());
                // 增加已释放的怪物数量
                mobs_released += 1;

                // 等待生成间隔时间
                yield return new WaitForSeconds(spawn_interval);
            }

            // 检查波次是否完成
            bool wave_completed = false;
            while (!wave_completed)
            {
                // 检查是否还有敌人存活
                bool enemy_left = false;
                foreach (MonsterWave w in sub_waves)
                {
                    if (w.mobs_released >= w.total_mobs_to_release)
                    {
                        foreach (Enemy e in w.mobs)
                        {
                            if (e.hp > 0)
                            {
                                enemy_left = true;
                            }
                        }
                    }
                    else
                    {
                        enemy_left = true;
                    }
                }
                // 更新波次完成状态
                wave_completed = !enemy_left;
                // 设置生成状态为false
                this.is_spawning = false;
                // 等待一帧
                yield return 0;
            }

            // 等待波次完成延迟时间
            float complete_delay = 3f;
            while (complete_delay > 0)
            {
                complete_delay -= Time.deltaTime;
            }
            // 触发波次完成事件
            EventWaveCompleted.Invoke();
            Debug.Log("波次完成");
        }

        // 波次类型枚举
        public enum WaveType
        {
            MainWave,
            SubWave
        }

        // 波次开始事件
        public UnityEvent EventWaveStart;
        // 延迟结束事件
        public UnityEvent EventDelayEnded;
        // 波次停止事件
        public UnityEvent EventWaveStop;
        // 波次完成事件
        public UnityEvent EventWaveCompleted;
        public bool IsAllWavesCompleted()
        {
            // 首先检查当前波次是否完成
            if (!IsWaveCompleted())
            {
                return false;  // 如果当前波次未完成，则返回false
            }

            // 检查所有子波次是否完成
            foreach (MonsterWave subWave in sub_waves)
            {
                if (subWave!= null &&!subWave.IsWaveCompleted())
                {
                    return false;  // 如果某个子波次未完成，则返回false
                }
            }

            // 如果当前波次和所有子波次都完成，则返回true
            return true;
        }

        private bool IsWaveCompleted()
        {
            // 如果当前波次已释放的怪物数量小于总数，波次还没有完成
            if (mobs_released < total_mobs_to_release)
            {
                return false;
            }

            // 检查所有怪物是否都死亡，如果有任何怪物存活，波次未完成
            foreach (Enemy mob in mobs)
            {
                if (mob!= null && mob.hp > 0) // 如果怪物存在且血量大于0，说明怪物还活着
                {
                    return false;
                }
            }

            // 如果所有怪物都死亡且已释放怪物数量达到预定目标，说明波次完成
            return true;
        }
        public UnityEvent EventGameSuccess;
        public void AllWavesCompleted(){
            if (IsAllWavesCompleted()){
                Debug.Log("所有波次已完成");
                EventGameSuccess.Invoke();
            }
        }
    }
}