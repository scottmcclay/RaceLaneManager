import { TestBed } from '@angular/core/testing';

import { RlmService } from './rlm.service';

describe('RlmService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: RlmService = TestBed.get(RlmService);
    expect(service).toBeTruthy();
  });
});
