import { Component, OnInit } from '@angular/core';
import { AppComponent } from '../app.component';
import { RlmService } from '../models/rlm.service';
import { Race } from '../models/race';

@Component({
  selector: 'app-lineup',
  templateUrl: './lineup.component.html',
  styleUrls: ['./lineup.component.scss']
})
export class LineupComponent implements OnInit {

  rlmService: RlmService;
  lineupResult: string;
  races: Array<Race>;

  constructor(service: RlmService) { 
    this.rlmService = service;
    this.races = new Array<Race>();
  }

  // constructor() { 
  // }

  ngOnInit() {
    //this.rlmService.GetLineup().subscribe((lineup) => { this.lineupResult = lineup});
    this.rlmService.GetRaces(8).subscribe((data: Array<any>) => {
      for (let r of data) {
        this.races.push(Race.fromPayload(r));
      }
    });
  }

}
