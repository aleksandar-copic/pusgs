import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerificateUserComponent } from './verificate-user.component';

describe('VerificateUserComponent', () => {
  let component: VerificateUserComponent;
  let fixture: ComponentFixture<VerificateUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerificateUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerificateUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
