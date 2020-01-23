import { Standing } from './standing';

export class GroupResult {
    groupName: string;
    standings: Array<Standing>;

    static fromPayload(payload: any): GroupResult {
        return {
            groupName: payload.GroupName,
            standings: Standing.getStandingsFromPayload(payload.Standings)
        };
    }
    
    static getGroupResultsFromPayload(payload: any): Array<GroupResult> {
        let results: Array<GroupResult> = new Array<GroupResult>();
        
        for (let gr of payload) {
            results.push(GroupResult.fromPayload(gr));
        }

        return results;
    }
}