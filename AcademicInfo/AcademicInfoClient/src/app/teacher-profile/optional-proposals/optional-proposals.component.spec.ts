import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OptionalProposalsComponent } from './optional-proposals.component';

describe('OptionalProposalsComponent', () => {
  let component: OptionalProposalsComponent;
  let fixture: ComponentFixture<OptionalProposalsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OptionalProposalsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OptionalProposalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
