#include <argp.h>
#include <arpa/inet.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include "cwiid.h"

const char *argp_program_version = "1.0";
const char *argp_program_address = "magickaito2141@gmail.com";
static char doc[] = "cwiid-based Wiimote data UDP emitter.";
static char args_doc[] ="[-p PORT] [-h HOST]";

static struct argp_option options[] = {
  {"port", 'p', "PORT", 0, "Specify target device port."},
  {"host", 'h', "HOST", 0, "Specify target device address."},
  { 0 }
};

char * device_ip = "10.20.0.2";
char * device_port = "9050";

static 
error_t 
parse_opt(int key, char *arg, struct argp_state *state)
{
  switch (key) {
    case 'p': 
      device_port = arg;
    break;
    case 'h':
      device_ip = arg;
    break;
  }
  return 0;
}

struct argp argp = { options, parse_opt, args_doc, doc, 0, 0, 0 };

// wiimote report receivers

static 
cwiid_mesg_callback_t cwiid_callback;

void cwiid_motionplus(
  struct cwiid_motionplus_mesg *mesg
);

void cwiid_acc(
  struct cwiid_acc_mesg *mesg
);

void cwiid_btn(
  struct cwiid_btn_mesg *mesg
);

//udp socket

int sock;
struct sockaddr_in dest_addr;
void setup_udp();
void send_data();

// out data
uint16_t phi, theta, psi,  x, y, z, btns;

int main(int argc, char **argv) {
  argp_parse (&argp, argc, argv, 0, 0, 0);
  setup_udp();
  cwiid_wiimote_t *wiimote = NULL;
  bdaddr_t bdaddr = *BDADDR_ANY;

  // char reset_bdaddr = 0;  
  // if (bacmp(&bdaddr, BDADDR_ANY) == 0) {
  //   reset_bdaddr = 1;
  // }

  printf("Put Wiimote in discoverable mode (press 1+2) and press OK\n");
  getchar();
  printf("Trying to connect...\n");
  if ((wiimote = cwiid_open(&bdaddr, CWIID_FLAG_MESG_IFC)) == NULL) {
    printf("No connection\n");
  } else if (cwiid_set_mesg_callback(wiimote, &cwiid_callback)) {
    printf("Error setting callback\n");
    if (cwiid_close(wiimote)) {
      printf("Error on disconnect\n");
    }
  } else {
    printf("Connected :D\n");
    cwiid_enable(wiimote, CWIID_FLAG_MOTIONPLUS);
    if (cwiid_set_rpt_mode(wiimote, CWIID_RPT_EXT | CWIID_RPT_ACC | CWIID_RPT_BTN)) {
      printf("Error setting report mode to motionplus");
    }
    // cwiid_request_status(wiimote);
    while (1) {}
    close(sock);
  }

}

static void
cwiid_callback(
  cwiid_wiimote_t *wiimote,
  int mesg_count,
  union cwiid_mesg mesg_array[],
  struct timespec *timestamp
) {
  int i;
  char *ext_str;
  static enum cwiid_ext_type ext_type = CWIID_EXT_NONE;
  for (i = 0; i < mesg_count; i++) {
    switch (mesg_array[i].type) { 
      case CWIID_MESG_MOTIONPLUS:
        cwiid_motionplus(&mesg_array[i].motionplus_mesg);
        break;
      case CWIID_MESG_ACC:
        cwiid_acc(&mesg_array[i].acc_mesg);
        break;
      case CWIID_MESG_BTN:
        cwiid_btn(&mesg_array[i].btn_mesg);
        break;
      default:
        break;
    }
  }
  send_data();
}


void cwiid_motionplus(struct cwiid_motionplus_mesg *mesg) {
  // printf("%X %X %X\n", mesg->angle_rate[CWIID_PHI], mesg->angle_rate[CWIID_THETA], mesg->angle_rate[CWIID_PSI]);
  phi = mesg->angle_rate[CWIID_PHI],
  theta = mesg->angle_rate[CWIID_THETA],
  psi = mesg->angle_rate[CWIID_PSI];
}

void cwiid_acc(struct cwiid_acc_mesg *mesg) {
  // printf("%X %X %X\n", mesg->angle_rate[CWIID_PHI], mesg->angle_rate[CWIID_THETA], mesg->angle_rate[CWIID_PSI]);
  x = mesg->acc[CWIID_X],
  y = mesg->acc[CWIID_Y],
  z = mesg->acc[CWIID_Z];
}

void cwiid_btn(struct cwiid_btn_mesg *mesg) {
  // printf("%X %X %X\n", mesg->angle_rate[CWIID_PHI], mesg->angle_rate[CWIID_THETA], mesg->angle_rate[CWIID_PSI]);
  btns = mesg->buttons;
  printf("%d\n", (int)(btns & CWIID_BTN_A));
}

void 
setup_udp() {
  sock = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
  memset(&dest_addr, 0, sizeof(dest_addr));
  dest_addr.sin_family = AF_INET;
  dest_addr.sin_port = htons(atoi(device_port));
  inet_pton(AF_INET, device_ip, &dest_addr.sin_addr);
}

void
send_data() {
  uint16_t packet[7] = {phi, theta, psi, x, y, z, htons(btns)};
  sendto(sock, packet, sizeof(packet), 0, (struct sockaddr*)&dest_addr, sizeof(dest_addr));
}
