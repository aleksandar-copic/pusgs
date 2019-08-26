import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CardVerificationComponent } from './card-verification.component';

describe('CardVerificationComponent', () => {
  let component: CardVerificationComponent;
  let fixture: ComponentFixture<CardVerificationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CardVerificationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CardVerificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
