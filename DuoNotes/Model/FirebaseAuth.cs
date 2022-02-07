using System;

namespace DuoNotes.Model {
    public class FirebaseUdes {
        public string localId { get; set; }
        public string federatedId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string displayName { get; set; }
        public string email { get; set; }
        public bool emailVerified { get; set; }
        public string photoUrl { get; set; }
        public string phoneNumber { get; set; }
    }

    public class FirebaseToken {
        public string idToken { get; set; }
        public string refreshToken { get; set; }
        public int expiresIn { get; set; }
        public DateTime Created { get; set; }
        public User User { get; set; }
    }
}
