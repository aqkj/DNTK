/* eslint-disable */
import type { BattlePassRewardTakeOption } from "./BattlePassRewardTakeOption";

/**
 * CmdId: 2602
 * EnetChannelId: 0
 * EnetIsReliable: true
 * IsAllowClient: true
 */
export interface TakeBattlePassRewardReq {
  TakeOptionList?: BattlePassRewardTakeOption[];
}