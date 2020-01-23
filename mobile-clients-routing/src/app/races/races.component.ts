import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RlmService } from '../models/rlm.service';
import { Race } from '../models/race';
import { Tournament } from '../models/tournament';

@Component({
  selector: 'app-races',
  templateUrl: './races.component.html',
  styleUrls: ['./races.component.scss']
})
export class RacesComponent implements OnInit {

  tournaments: Array<Tournament>;
  races: Array<Race>;
  selectedTournamentName: string;

  constructor(
    private route: ActivatedRoute,
    private rlmService: RlmService
  ) { }

  ngOnInit() {
    this.tournaments = new Array<Tournament>();
    this.races = new Array<Race>();

    const tournamentId = +this.route.snapshot.paramMap.get('id');

    this.rlmService.GetTournaments().subscribe((data: Array<any>) => {
      for (let t of data) {
        let tournament: Tournament = Tournament.fromPayload(t);
        this.tournaments.push(tournament);

        if (tournament.id == tournamentId) {
          this.selectedTournamentName = tournament.name;
        }
      }
    });

    this.rlmService.GetRaces(tournamentId).subscribe((data: Array<any>) => {
      for (let r of data) {
        this.races.push(Race.fromPayload(r));
      }
    });
  }
}
